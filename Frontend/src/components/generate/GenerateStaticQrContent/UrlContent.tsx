import { Stack, TextField } from "@mui/material"
import { useEffect, useState } from "react"
import { GenerateStaticQRContentProps } from "./GenerateStaticQRContent"

const UrlContent = ({ setValue }: GenerateStaticQRContentProps) => {

  const [rawContent, setRawContent] = useState("https://www.google.com")

  useEffect(() => {
    setValue(rawContent)
  }, [rawContent])

  return <>
    <Stack spacing={2}>
      <TextField
        size="small"
        multiline
        fullWidth
        label="Url"
        value={rawContent}
        onChange={(e) => setRawContent(e.target.value)}
      />
    </Stack>
  </>
}

export default UrlContent
