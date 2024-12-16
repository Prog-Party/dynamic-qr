import axios from "axios"
import { constructUrl } from "../ApiHelper"

export const getCode = async (organizationIdentifier: string, qrCodeId: string) => {

  try {
    const url = constructUrl(`qr-codes/${qrCodeId}`)
    const response = await axios.get(url, {
      headers: {
        "Content-Type": "application/json",
        "Organization-Identifier": organizationIdentifier
      }
    })
    return response.data
  } catch (error) {
    throw error
  }
}