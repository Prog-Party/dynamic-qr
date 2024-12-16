export function calculateNewQrCodeId(): string {
  //TODO: Implement this function by calling an API to get a new QR code ID
  const amountCharacters = 10

  return generateRandomId(amountCharacters)
}

function generateRandomId(amountCharacters: number): string {
  // Generate a random alphanumeric ID
  const characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"
  let randomId = ""

  for (let i = 0; i < amountCharacters; i++)
    randomId += characters.charAt(Math.floor(Math.random() * characters.length))

  return randomId
}