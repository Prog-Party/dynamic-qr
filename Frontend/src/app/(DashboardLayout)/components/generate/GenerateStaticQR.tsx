import ColorPickerWithPopover from "@/app/components/ColorPickerWithPopover";
import calculateContrastRatio from "@/utils/colors/calculateContrastRatio";
import { FormControlLabel, Grid, Stack, Switch, TextField, Typography } from "@mui/material";
import Alert from '@mui/material/Alert';
import { QRCodeSVG } from 'qrcode.react';
import { useEffect, useState } from 'react';
import { QRProps } from './GenerateStaticQR.d';

const GenerateStaticQr = () => {
    const [value, setValue] = useState("Welcome world") //TODO: Make a more fun default text value
    const [includeMargin, setMargin] = useState(false)
    const [backgroundColor, setBackgroundColor] = useState("#ffffff")
    const [foregroundColor, setForegroundColor] = useState("#000000")

    /*
     * The minimum contrast for the background and foreground color of a QR code is typically defined by the contrast ratio between the two colors. To ensure reliable scanning, the contrast ratio should be at least 4:1.
     * However, for optimal readability and to meet most QR code standards, a higher contrast ratio is recommended. The ideal is a contrast ratio of 7:1 or higher. This typically means that the foreground (often the QR code itself) is much darker than the background.
     */
    const [contrastRatio, setContrastRatio] = useState(4)

    const qrCodePropsDefault: QRProps = {
        value: value
    };

    const [qrCodeProps, setQrCodeProps] = useState(qrCodePropsDefault)

    useEffect(() => {
        setQrCodeProps({
            value: value,
            includeMargin: includeMargin,
            bgColor: backgroundColor,
            fgColor: foregroundColor,
            size: 156, //TODO: Make this configurable, when clicking on the qr code, there should be a popout to configure the barcode and download it, also change its size
            level: 'H' //TODO: Make this configurable,
            //TODO: make this configurable
            // imageSettings: {
            //     src: 'https://upload.wikimedia.org/wikipedia/commons/a/a7/React-icon.svg',
            //     height: 24,
            //     width: 24,
            //     excavate: true,
            // }
        })
    }, [value, includeMargin, foregroundColor, backgroundColor])

    useEffect(() => {
        const contrast = calculateContrastRatio(backgroundColor, foregroundColor)
        setContrastRatio(contrast)
    }, [backgroundColor, foregroundColor])

    return <>
        <Grid container spacing={4} sx={{ mt: 0 }}>
            <Grid item xs={12} lg={2} >
                <QRCodeSVG {...qrCodeProps} />
            </Grid>
            <Grid item xs={12} lg={4} >
                <Stack spacing={2}>
                    <Typography variant="h2" color="textSecondary" >
                        Content
                    </Typography>
                    {/* TODO: Add options so an URL can be added, VCard, E-mail, etc. */}
                    <TextField
                        fullWidth
                        label="Value"
                        value={value}
                        onChange={(e) => setValue(e.target.value)}
                    />
                </Stack>
            </Grid>
            <Grid item xs={12} lg={6}>
                <Stack spacing={2}>
                    <Typography variant="h2" color="textSecondary">
                        Design
                    </Typography>

                    <Grid
                        container
                        spacing={1}
                        alignItems="center"
                    >
                        <Grid item lg={3}>
                            <Typography variant="body1" color="textSecondary">
                                Include margin
                            </Typography>
                        </Grid>
                        <Grid item lg={9}>
                            <FormControlLabel
                                control={
                                    <Switch
                                        checked={includeMargin}
                                        onChange={(e) => setMargin(e.target.checked)}
                                    />
                                }
                                label=""
                            />
                        </Grid>

                        <Grid item lg={3}>
                            <Typography variant="body1" color="textSecondary">
                                Colors
                            </Typography>
                        </Grid>
                        <Grid item lg={9}>
                            <Stack direction="row" spacing={2}>
                                <ColorPickerWithPopover
                                    color={backgroundColor}
                                    onChange={(color: string) => setBackgroundColor(color)} />
                                <ColorPickerWithPopover
                                    color={foregroundColor}
                                    onChange={(color: string) => setForegroundColor(color)} />
                            </Stack>
                        </Grid>

                        {contrastRatio <= 7 && (
                            <>
                                <Grid item lg={12}>
                                    <Alert severity={contrastRatio <= 4 ? "error" : "warning"}>
                                        The current contrast ratio is <b>{contrastRatio.toFixed(0)}:1</b>
                                        <br />A minimum of 4:1 is recommended
                                        <br />Higher than 7:1 is ideal
                                    </Alert>
                                </Grid>
                            </>
                        )}
                    </Grid>
                </Stack>
            </Grid>
        </Grid>
        {/* TODO: Add a download button to download the qr code */}
        {/* TODO: Add a button to make this qr code dynamic, by clicking on the button, the configuration should be saved and the qr code should be dynamic. Somehow the user must then be able to login to save it in their account */}
    </>
};

export default GenerateStaticQr;
