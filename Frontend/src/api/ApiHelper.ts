
const API_BASE_URL = "https://dynamic-qrs.azurewebsites.net/api"

export const constructUrl = (endpoint: string) => {
  const backendKey = process.env.BACKEND_KEY_QUERY
  // if endpoint contains a questionmark, append with & else append with ?
  const separator = endpoint.includes("?") ? "&" : "?"

  return `${API_BASE_URL}/${endpoint}${separator}${backendKey}`
}