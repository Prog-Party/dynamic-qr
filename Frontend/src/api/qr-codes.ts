import axios from "axios"
import { constructUrl } from "./ApiHelper"

export const getCodes = async (organizationIdentifier: string) => {

  try {
    const url = constructUrl("qr-codes")
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

export const createCode = async (organizationIdentifier: string, value: string) => {

  try {
    const url = constructUrl("qr-codes")
    const data = {
      Value: value
    }
    const response = await axios.post(url, data, {
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