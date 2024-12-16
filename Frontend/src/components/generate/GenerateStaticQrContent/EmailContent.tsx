import { Stack, TextField } from "@mui/material"
import { useEffect, useState } from "react"
import { GenerateStaticQRContentProps } from "./GenerateStaticQRContent"

const EmailContent = ({ setValue }: GenerateStaticQRContentProps) => {

  const [email, setEmail] = useState("some@someone.com")
  const [subject, setSubject] = useState("")
  const [body, setBody] = useState("")

  const generateMailtoLink = (): string => {
    let mailtoLink = `mailto:${email}`
    const queryParams = []

    if (subject)
      queryParams.push(`subject=${encodeURIComponent(subject)}`)

    if (body)
      queryParams.push(`body=${encodeURIComponent(body)}`)

    if (queryParams.length > 0)
      mailtoLink += `?${queryParams.join("&")}`

    return mailtoLink
  }

  useEffect(() => {
    setValue(generateMailtoLink())
  }, [email, subject, body])

  return <>
    <Stack spacing={2}>
      <TextField
        size="small"
        fullWidth
        label="Email"
        value={email}
        onChange={(e) => setEmail(e.target.value)}
      />
      <TextField
        size="small"
        fullWidth
        label="Subject"
        value={subject}
        onChange={(e) => setSubject(e.target.value)}
      />
      <TextField
        size="small"
        multiline
        fullWidth
        label="Body"
        value={body}
        onChange={(e) => setBody(e.target.value)}
      />
    </Stack>
  </>
}

export default EmailContent
