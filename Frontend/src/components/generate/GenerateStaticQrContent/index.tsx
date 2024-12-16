import CardGiftcardIcon from "@mui/icons-material/CardGiftcard"
import EmailIcon from "@mui/icons-material/Email"
import PhoneAndroidIcon from "@mui/icons-material/PhoneAndroid"
import SmsIcon from "@mui/icons-material/Sms"
import WebIcon from "@mui/icons-material/Web"
import Wifi2BarIcon from "@mui/icons-material/Wifi2Bar"
import { Box, Grid, Stack, ToggleButton, ToggleButtonGroup, Typography } from "@mui/material"
import { useState } from "react"
import EmailContent from "./EmailContent"
import { GenerateStaticQRContentProps } from "./GenerateStaticQRContent"
import PhoneContent from "./PhoneContent"
import PlainContent from "./PlainContent"
import SmsContent from "./SmsContent"
import UrlContent from "./UrlContent"
import VCardContent from "./VCardContent"
import WifiContent from "./WifiContent"
//TODO: Location: Geolocation coordinates (geo:latitude,longitude).
//TODO: Event: Calendar events (BEGIN:VEVENT).

const GenerateStaticQrContent = ({ value, setValue }: GenerateStaticQRContentProps) => {

  const options = [
    { value: "plain", label: "Plain", Type: PlainContent },
    {
      value: "website", label: <>
        <WebIcon />
        <Typography variant="body1">
                    Website
        </Typography>
      </>, Type: UrlContent
    },
    {
      value: "email", label: <>
        <EmailIcon />
        <Typography variant="body1">
                    E-mail
        </Typography>
      </>, Type: EmailContent
    },
    {
      value: "wifi", label: <>
        <Wifi2BarIcon />
        <Typography variant="body1">
                    Wifi
        </Typography>
      </>, Type: WifiContent
    },
    {
      value: "phone", label: <>
        <PhoneAndroidIcon />
        <Typography variant="body1">
                    Phone
        </Typography>
      </>, Type: PhoneContent
    },
    {
      value: "sms", label: <>
        <SmsIcon />
        <Typography variant="body1">
                    SMS
        </Typography>
      </>, Type: SmsContent
    },
    {
      value: "vcard", label: <>
        <CardGiftcardIcon />
        <Typography variant="body1">
                    VCard
        </Typography>
      </>, Type: VCardContent
    }
  ]

  const [alignment, setAlignment] = useState(options[0].value)

  const handleChange = (
    event: React.MouseEvent<HTMLElement>,
    newAlignment: string,
  ) => {
    setAlignment(newAlignment)
  }

  return <>
    <Stack spacing={2}>
      <Typography variant="h2" color="textSecondary" >
                Content
      </Typography>

      <ToggleButtonGroup
        color="primary"
        value={alignment}
        exclusive
        onChange={handleChange}
      >
        <Grid container>
          {options.map(({ value, label }) => (
            <Grid
              item
              lg={4}
              key={value}>
              <ToggleButton
                sx={{ minWidth: "100px", mb: 1 }}
                value={value}
                aria-label={value}>
                {label}
              </ToggleButton>
            </Grid>
          ))}

        </Grid>
      </ToggleButtonGroup>

      <Box
        component={options.find(x => x.value === alignment)!.Type}
        value={value}
        setValue={setValue}
      />
    </Stack>
  </>
}

export default GenerateStaticQrContent
