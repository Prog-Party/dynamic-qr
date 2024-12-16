import LoginButton from "@/components/auth/LoginButton"
import Loading from "@/components/Loading"
import { useAuth0 } from "@auth0/auth0-react"
import { AppBar, Badge, Box, IconButton, List, Stack, styled, Toolbar, useMediaQuery } from "@mui/material"
import { IconBellRinging, IconMenu } from "@tabler/icons-react"
import Link from "next/link"
import { usePathname } from "next/navigation"
import PropTypes from "prop-types"
import React from "react"
import { Logo } from "react-mui-sidebar"
import Menuitems from "./MenuItems"
import NavItem from "./NavItem"
import Profile from "./Profile"

interface ItemType {
  toggleMobileSidebar: (event: React.MouseEvent<HTMLElement>) => void;
}

const Header = ({ toggleMobileSidebar }: ItemType) => {

  const { user, isAuthenticated, isLoading } = useAuth0()

  const lgUp = useMediaQuery((theme: any) => theme.breakpoints.up("lg"))
  // const lgDown = useMediaQuery((theme) => theme.breakpoints.down('lg'));

  const AppBarStyled = styled(AppBar)(({ theme }) => ({
    boxShadow: "none",
    background: theme.palette.background.paper,
    justifyContent: "center",
    backdropFilter: "blur(4px)",
    [theme.breakpoints.up("lg")]: {
      minHeight: "70px",
    },
  }))
  const ToolbarStyled = styled(Toolbar)(({ theme }) => ({
    width: "100%",
    color: theme.palette.text.secondary,
  }))

  const pathname = usePathname()
  const pathDirect = pathname || "/"

  if (isLoading)
    return <Loading />

  return (
    <AppBarStyled position="sticky" color="default">
      <ToolbarStyled>
        <IconButton
          color="inherit"
          aria-label="menu"
          onClick={toggleMobileSidebar}
          sx={{
            display: {
              lg: "none",
              xs: "inline",
            },
          }}
        >
          <IconMenu width="20" height="20" />
        </IconButton>

        <IconButton
          size="large"
          aria-label="show 11 new notifications"
          color="inherit"
          aria-controls="msgs-menu"
          aria-haspopup="true"
        >
          <Badge variant="dot" color="primary">
            <IconBellRinging size="21" stroke="1.5" />
          </Badge>

        </IconButton>

        {/* ------------------------------------------- */}
        {/* Logo */}
        {/* ------------------------------------------- */}
        {lgUp && !isAuthenticated && (
          <Link href="/">
            <Logo img="/images/logos/Logo-64x64.png" />
          </Link>
        )}

        {lgUp && (
          <Box sx={{ px: 3 }}>
            <List sx={{ pt: 0 }} className="sidebarNav" component="div">
              {Menuitems.map((item) => {
                return (
                  <NavItem
                    item={item}
                    key={item.id}
                    pathDirect={pathDirect}
                    onClick={toggleMobileSidebar}
                  />
                )
              })}
            </List>
          </Box>)}

        <Box flexGrow={1} />

        <Stack spacing={1} direction="row" alignItems="center">

          {!isAuthenticated && <LoginButton />}
          {isAuthenticated && (
            <>
              <Profile />
            </>)}
        </Stack>
      </ToolbarStyled>
    </AppBarStyled>
  )
}

Header.propTypes = {
  sx: PropTypes.object,
}

export default Header
