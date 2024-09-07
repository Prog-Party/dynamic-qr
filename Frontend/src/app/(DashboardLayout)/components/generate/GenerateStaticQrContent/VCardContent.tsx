import { Stack, TextField } from "@mui/material";
import { useEffect, useState } from "react";
import { GenerateStaticQRContentProps } from "./GenerateStaticQRContent";

const VCardContent = ({ setValue }: GenerateStaticQRContentProps) => {

    // See for all options: https://en.wikipedia.org/wiki/VCard
    const [firstName, setFirstName] = useState("John")
    const [lastName, setLastName] = useState("")
    const [logoUrl, setLogoUrl] = useState("")
    const [note, setNote] = useState("")
    const [photoUrl, setPhotoUrl] = useState("")
    const [organisation, setOrganisation] = useState("")
    const [phone, setPhone] = useState("")
    const [jobTitle, setJobTitle] = useState("")
    const [website, setWebsite] = useState("")
    const [email, setEmail] = useState("")
    const [categories, setCategories] = useState("")
    const [dateOfBirth, setDateOfBirth] = useState("")
    //TODO: Geo
    //TODO: Address

    function generateGuid() {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            const r = Math.random() * 16 | 0;
            const v = c === 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    }

    const getImageType = (url: string) => {
        if (url.endsWith(".png"))
            return "PNG"
        if (url.endsWith(".jpg") || url.endsWith(".jpeg"))
            return "JPEG"
        if (url.endsWith(".gif"))
            return "GIF"
        return "PNG"
    }

    const generateVcardContent = (): string => {
        let content = `BEGIN:VCARD\n`
        content += `VERSION:4.0\n`

        if (firstName || lastName) {
            content += `FN:${firstName ?? ""} ${lastName ?? ""}\n`
            content += `N:${lastName ?? ""};${firstName ?? ""}\n`
        }

        // ADR;WORK;PREF;QUOTED-PRINTABLE:;Footown 12345=0AFooland;Bar Street 99
        // LABEL;QUOTED-PRINTABLE;WORK;PREF:Bar Street 99=0AFootown 12345=0AFooland
        if (logoUrl)
            content += `LOGO;TYPE=${getImageType(logoUrl)}:${logoUrl}\n`
        if (photoUrl)
            content += `PHOTO;TYPE=${getImageType(photoUrl)};VALUE=URI:${photoUrl}\n`
        if (note)
            content += `NOTE:${note}\n`
        if (organisation)
            content += `ORG:${organisation}\n`
        if (phone)
            content += `TEL:${phone}\n`
        if (jobTitle)
            content += `TITLE:${jobTitle}\n`
        if (website)
            content += `URL:${website}\n`
        if (email)
            content += `EMAIL:${email}\n`
        if (categories)
            content += `CATEGORIES:${categories}\n`
        if (dateOfBirth)
            content += `BDAY:${dateOfBirth}\n`
        content += "UID:urn:uuid" + generateGuid() + "\n"
        content += "END:VCARD"

        return content;
    };

    useEffect(() => {
        setValue(generateVcardContent())
    }, [firstName, lastName, logoUrl, note, photoUrl, organisation, phone, jobTitle, website, email, categories, dateOfBirth])

    return <>
        <Stack spacing={2}>
            <TextField
                size="small"
                fullWidth
                label="First name"
                value={firstName}
                onChange={(e) => setFirstName(e.target.value)}
            />
            <TextField
                size="small"
                fullWidth
                label="Last name"
                value={lastName}
                onChange={(e) => setLastName(e.target.value)}
            />
            <TextField
                size="small"
                fullWidth
                label="Logo URL"
                value={logoUrl}
                onChange={(e) => setLogoUrl(e.target.value)}
            />
            <TextField
                size="small"
                fullWidth
                label="Photo URL"
                value={photoUrl}
                onChange={(e) => setPhotoUrl(e.target.value)}
            />
            <TextField
                size="small"
                fullWidth
                label="Organisation"
                value={organisation}
                onChange={(e) => setOrganisation(e.target.value)}
            />
            <TextField
                size="small"
                fullWidth
                label="Job title"
                value={jobTitle}
                onChange={(e) => setJobTitle(e.target.value)}
            />
            <TextField
                size="small"
                fullWidth
                label="Phone"
                value={phone}
                onChange={(e) => setPhone(e.target.value)}
            />
            <TextField
                size="small"
                fullWidth
                label="Website"
                value={website}
                onChange={(e) => setWebsite(e.target.value)}
            />
            <TextField
                size="small"
                multiline
                fullWidth
                label="Note"
                value={note}
                onChange={(e) => setNote(e.target.value)}
            />
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
                label="Categories (comma separated)"
                value={categories}
                onChange={(e) => setCategories(e.target.value)}
            />
            <TextField
                size="small"
                fullWidth
                label="Date of birth"
                value={dateOfBirth}
                onChange={(e) => setDateOfBirth(e.target.value)}
            />
        </Stack>
    </>
};

export default VCardContent;
