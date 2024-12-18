import { FormControl, FormControlLabel, InputLabel, MenuItem, Select, Stack, Switch, TextField } from "@mui/material"
import { useEffect, useState } from "react"
import { GenerateStaticQRContentProps } from "./GenerateStaticQRContent"

enum SecurityOptions {
  NOPASS = "nopass",
  WEP = "WEP",
  WPA = "WPA"
}

const WifiContent = ({ setValue }: GenerateStaticQRContentProps) => {

  const [ssid, setSsid] = useState("")
  const [password, setPassword] = useState("")

  const [security, setSecurity] = useState(SecurityOptions.WPA.toString())
  const [hidden, setHidden] = useState(false)

  const generateContent = (): string => {
    const pass = security === SecurityOptions.NOPASS.toString() ? "password" : password
    const result = `WIFI:S:${ssid};T:${security};P:${pass};H:${hidden};;`
    return result
  }

  useEffect(() => {
    setValue(generateContent())
  }, [ssid, password, security, hidden])

  return <>
    <Stack spacing={2}>
      <TextField
        size="small"
        fullWidth
        label="SSID / Network name"
        value={ssid}
        onChange={(e) => setSsid(e.target.value)}
      />

      <FormControl size="small" fullWidth>
        <InputLabel>Security</InputLabel>
        <Select
          value={security}
          onChange={(e) => setSecurity(e.target.value)}
        >
          <MenuItem value={SecurityOptions.WPA.toString()}>WPA/WPA2</MenuItem>
          <MenuItem value={SecurityOptions.WEP.toString()}>WEP</MenuItem>
          <MenuItem value={SecurityOptions.NOPASS.toString()}>No Password</MenuItem>
        </Select>
      </FormControl>

      {security !== SecurityOptions.NOPASS.toString() && (
        <TextField
          size="small"
          fullWidth
          label="Password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
        />
      )}

      <FormControlLabel
        control={
          <Switch
            checked={hidden}
            onChange={(e) => setHidden(e.target.checked)}
          />
        }
        label="Hidden"
      />
    </Stack>
  </>
}

export default WifiContent
