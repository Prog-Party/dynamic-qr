"use client"
import PageContainer from "@/components/container/PageContainer"
import DashboardCard from "@/components/shared/DashboardCard"
import { useAuth0 } from "@auth0/auth0-react"
import { Typography } from "@mui/material"
import { useEffect } from "react"

/**
 * LogoutPage component.
 *
 * This component renders a page for logging out the user.
 *
 * Sometimes the user is logged in while the frontend doesn't know it yet.
 * Then the logout button isn't visible
 * Therefore this page exists, to ensure people can always explicitly logout
 *
 * @returns The rendered LogoutPage component.
 */
const LogoutPage = () => {

  const { logout } = useAuth0()

  useEffect(() => {
    //Logout when the page loads
    logout()
  }, [])

  return (
    <PageContainer title="Log out" description="Log out">
      <DashboardCard title="Logging out">
        <Typography>
          We are logging you out.
        </Typography>
      </DashboardCard>
    </PageContainer>
  )
}

export default LogoutPage

