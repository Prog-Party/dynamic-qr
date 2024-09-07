import ColorPickerWithPopover from "@/app/components/ColorPickerWithPopover";
import calculateContrastRatio from "@/utils/colors/calculateContrastRatio";
import InfoIcon from '@mui/icons-material/Info';
import { FormControlLabel, Grid, Slider, Stack, Switch, Table, TableBody, TableCell, TableContainer, TableRow, TextField, Typography } from "@mui/material";
import Alert from '@mui/material/Alert';
import { styled } from '@mui/material/styles';
import Tooltip from '@mui/material/Tooltip';
import { QRCodeSVG } from 'qrcode.react';
import { useEffect, useState } from 'react';
import { QRProps } from './GenerateStaticQR.d';

const SlimTableCell = styled(TableCell)(({ theme }) => ({
    paddingTop: '0px',
    paddingLeft: '0px',
    paddingBottom: '12px',
    paddingRight: '12px'
}));

const GenerateStaticQr = () => {
    const [value, setValue] = useState("Welcome world") //TODO: Make a more fun default text value
    const [includeMargin, setMargin] = useState(false)
    const [backgroundColor, setBackgroundColor] = useState("#ffffff")
    const [foregroundColor, setForegroundColor] = useState("#000000")
    const [imageUrl, setImageUrl] = useState("https://upload.wikimedia.org/wikipedia/commons/a/a7/React-icon.svg")
    const [imageHeight, setImageHeight] = useState(15) //in percentage
    const [imageWidth, setImageWidth] = useState(15)// in percentage
    const [imageHeightSameAsWidth, setImageHeightSameAsWidth] = useState(true)

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
        if (imageHeightSameAsWidth)
            setImageHeight(imageWidth)
    }, [imageWidth, imageHeightSameAsWidth])

    useEffect(() => {
        const size = 156
        var props: QRProps = {
            value: value,
            includeMargin: includeMargin,
            bgColor: backgroundColor,
            fgColor: foregroundColor,
            size: size, //TODO: Make this configurable, when clicking on the qr code, there should be a popout to configure the barcode and download it, also change its size
            level: 'H' //TODO: Make this configurable,
        }

        if (imageUrl) {
            const width = size * (imageWidth / 100)
            const height = imageHeightSameAsWidth ? width : (size * (imageHeight / 100))

            props.imageSettings = {
                src: imageUrl,
                height: height,
                width: width,
                excavate: true,
            }
        }

        setQrCodeProps(props)
    }, [value, includeMargin, foregroundColor, backgroundColor, imageUrl, imageHeight, imageWidth])

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
                <Typography variant="h2" color="textSecondary">
                    Design
                </Typography>

                <TableContainer>
                    <Table aria-label="simple table">
                        <TableBody>
                            <TableRow>
                                <SlimTableCell width="25%">
                                    <Typography variant="body1" color="textSecondary">
                                        Include margin
                                    </Typography>
                                </SlimTableCell>
                                <SlimTableCell>
                                    <FormControlLabel
                                        control={
                                            <Switch
                                                checked={includeMargin}
                                                onChange={(e) => setMargin(e.target.checked)}
                                            />
                                        }
                                        label=""
                                    />
                                </SlimTableCell>
                            </TableRow>
                            <TableRow>
                                <SlimTableCell>
                                    <Typography variant="body1" color="textSecondary">
                                        Colors
                                    </Typography>
                                </SlimTableCell>
                                <SlimTableCell>
                                    <Stack direction="row" spacing={2}>
                                        <ColorPickerWithPopover
                                            size="24px"
                                            color={backgroundColor}
                                            onChange={(color: string) => setBackgroundColor(color)} />
                                        <ColorPickerWithPopover
                                            size="24px"
                                            color={foregroundColor}
                                            onChange={(color: string) => setForegroundColor(color)} />
                                    </Stack>
                                </SlimTableCell>
                            </TableRow>

                            {contrastRatio <= 7 && (
                                <TableRow>
                                    <SlimTableCell colSpan={2}>
                                        <Alert severity={contrastRatio <= 4 ? "error" : "warning"}>
                                            The current contrast ratio is <b>{contrastRatio.toFixed(0)}:1</b>
                                            <br />A minimum of 4:1 is recommended
                                            <br />Higher than 7:1 is ideal
                                        </Alert>
                                    </SlimTableCell>
                                </TableRow>
                            )}

                            <TableRow>
                                <SlimTableCell>
                                    <Stack
                                        direction="row"
                                        spacing={2}
                                        justifyContent="space-between"
                                    >
                                        <Typography variant="body1" color="textSecondary">
                                            Image
                                        </Typography>

                                        <Typography variant="body1" color="textSecondary">
                                            <Tooltip title="Decorate the QR code with an image">
                                                <InfoIcon />
                                            </Tooltip>
                                        </Typography>
                                    </Stack>
                                </SlimTableCell>
                                <SlimTableCell>
                                    <TextField
                                        fullWidth
                                        label="Image URL"
                                        value={imageUrl}
                                        size="small"
                                        onChange={(e) => setImageUrl(e.target.value)}
                                    />
                                </SlimTableCell>
                            </TableRow>

                            {imageUrl && (

                                <TableRow>
                                    <SlimTableCell>
                                        <Stack
                                            direction="row"
                                            spacing={2}
                                            justifyContent="space-between"
                                        >
                                            <Typography variant="body1" color="textSecondary">
                                                Image size (in %)
                                            </Typography>
                                        </Stack>
                                    </SlimTableCell>
                                    <SlimTableCell>
                                        <Typography variant="body1">
                                            Width
                                        </Typography>
                                        <Slider
                                            sx={{ width: '90%' }}
                                            aria-label="Width"
                                            defaultValue={imageWidth}
                                            valueLabelDisplay="auto"
                                            shiftStep={5}
                                            step={1}
                                            marks
                                            min={10}
                                            max={25}
                                            onChange={(e, value) => setImageWidth(value as number)}
                                        />

                                        <Typography variant="body1">
                                            Height
                                        </Typography>
                                        <FormControlLabel
                                            control={
                                                <Switch
                                                    checked={imageHeightSameAsWidth}
                                                    onChange={(e) => setImageHeightSameAsWidth(e.target.checked)}
                                                />
                                            }
                                            label="Same as width"
                                        />

                                        {!imageHeightSameAsWidth && (
                                            <Slider
                                                sx={{ width: '90%' }}
                                                aria-label="Height"
                                                defaultValue={imageHeight}
                                                valueLabelDisplay="auto"
                                                shiftStep={5}
                                                step={1}
                                                marks
                                                min={10}
                                                max={25}
                                                onChange={(e, value) => setImageHeight(value as number)}
                                            />
                                        )}
                                    </SlimTableCell>
                                </TableRow>
                            )}
                        </TableBody>
                    </Table>
                </TableContainer>
            </Grid >
        </Grid >
        {/* TODO: Add a download button to download the qr code */}
        {/* TODO: Add a button to make this qr code dynamic, by clicking on the button, the configuration should be saved and the qr code should be dynamic. Somehow the user must then be able to login to save it in their account */}
    </>
};

export default GenerateStaticQr;
