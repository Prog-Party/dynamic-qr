import CardGiftcardIcon from "@mui/icons-material/CardGiftcard";
import EmailIcon from "@mui/icons-material/Email";
import { Box, Stack, ToggleButton, ToggleButtonGroup, Typography } from "@mui/material";
import { useState } from "react";
import EmailContent from "./EmailContent";
import { GenerateStaticQRContentProps } from "./GenerateStaticQRContent";
import PlainContent from "./PlainContent";
import VCardContent from "./VCardContent";

const GenerateStaticQrContent = ({ value, setValue }: GenerateStaticQRContentProps) => {

    const options = [
        { value: 'plain', label: 'Plain', Type: PlainContent },
        {
            value: 'email', label: <>
                <EmailIcon />
                <Typography variant="body1">
                    E-mail
                </Typography>
            </>, Type: EmailContent
        },
        {
            value: 'vcard', label: <>
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
        setAlignment(newAlignment);
    };

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
                {options.map(({ value, label }) => (
                    <ToggleButton key={value} value={value} aria-label={value}>
                        {label}
                    </ToggleButton>
                ))}
            </ToggleButtonGroup>

            <Box
                component={options.find(x => x.value === alignment)!.Type}
                value={value}
                setValue={setValue}
            />
        </Stack>
    </>
};

export default GenerateStaticQrContent;
