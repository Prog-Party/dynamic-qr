import { styled } from "@mui/material"
import Image from "next/image"
import Link from "next/link"

const LinkStyled = styled(Link)(() => ({
  height: "70px",
  width: "180px",
  overflow: "hidden",
  display: "block",
}))

const Logo = () => {
  return (
    <LinkStyled href="/">
      <Image src="/images/logos/Logo-64x64.png" alt="logo" height={64} width={64} priority />
    </LinkStyled>
  )
}

export default Logo
