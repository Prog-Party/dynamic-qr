import { Stack, TextField } from "@mui/material"
import { useEffect, useState } from "react"
import { GenerateStaticQRContentProps } from "./GenerateStaticQRContent"

const PhoneContent = ({ setValue }: GenerateStaticQRContentProps) => {

  const [phoneNumber, setPhoneNumber] = useState("")

  const generateContent = (): string => {
    const result = `tel:${phoneNumber}`
    return result
  }

  useEffect(() => {
    setValue(generateContent())
  }, [phoneNumber])

  return <>
    <Stack spacing={2}>
      <TextField
        size="small"
        fullWidth
        label="Phone Number"
        value={phoneNumber}
        onChange={(e) => setPhoneNumber(e.target.value)}
      />
    </Stack>
  </>
}

export default PhoneContent
