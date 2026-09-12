export function parseJwt() {
  try {
    const token = localStorage.getItem("token");
    if (!token) return null;

    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    
    const jsonPayload = decodeURIComponent(
      window.atob(base64)
        .split('')
        .map(c => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
        .join('')
    );

const parsedJson = JSON.parse(jsonPayload);

    // Access claims using .NET's default XML namespace URI keys
    const fullName = parsedJson["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"];
    const userId = parsedJson["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];
    const role = parsedJson["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
    const jti = parsedJson["jti"]; // Standard short key (JwtRegisteredClaimNames.Jti)

    localStorage.setItem("fullName", fullName);
    localStorage.setItem("userId", userId);
    localStorage.setItem("role", role);
    localStorage.setItem("jti", jti);

    return parsedJson;
  } catch (error) {
    console.error("Invalid token", error);
    return null;
  }
}