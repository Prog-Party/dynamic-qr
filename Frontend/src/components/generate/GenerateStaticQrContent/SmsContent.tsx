import { Stack, TextField } from "@mui/material"
import { useEffect, useState } from "react"
import { GenerateStaticQRContentProps } from "./GenerateStaticQRContent"

const SmsContent = ({ setValue }: GenerateStaticQRContentProps) => {

  const [phoneNumber, setPhoneNumber] = useState("")
  const [message, setMessage] = useState("")

  const generateContent = (): string => {
    const result = `sms:${phoneNumber}:${message}`
    return result
  }

  useEffect(() => {
    setValue(generateContent())
  }, [phoneNumber, message])

  return <>
    <Stack spacing={2}>
      <TextField
        size="small"
        fullWidth
        label="Phone Number"
        value={phoneNumber}
        onChange={(e) => setPhoneNumber(e.target.value)}
      />
      <TextField
        size="small"
        fullWidth
        label="Message"
        value={message}
        onChange={(e) => setMessage(e.target.value)}
      />
    </Stack>
  </>
}

export default SmsContent
