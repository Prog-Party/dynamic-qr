import { Stack, TextField } from "@mui/material"
import { useEffect, useState } from "react"
import { GenerateStaticQRContentProps } from "./GenerateStaticQRContent"

const PlainContent = ({ setValue }: GenerateStaticQRContentProps) => {

  //TODO: Make a more fun default text value
  const [rawContent, setRawContent] = useState("Welcome world")

  useEffect(() => {
    setValue(rawContent)
  }, [rawContent])

  return <>
    <Stack spacing={2}>
      <TextField
        size="small"
        multiline
        fullWidth
        label="Raw content"
        value={rawContent}
        onChange={(e) => setRawContent(e.target.value)}
      />
    </Stack>
  </>
}

export default PlainContent
