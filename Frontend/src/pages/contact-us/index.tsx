"use client"
import PageContainer from "@/components/container/PageContainer"
import DashboardCard from "@/components/shared/DashboardCard"
import { useAuth0 } from "@auth0/auth0-react"
import { Typography } from "@mui/material"

const ContactUsPage = () => {

  const { user, isAuthenticated } = useAuth0()

  return (
    <PageContainer title="Contact us" description="this is Contact us page">
      <DashboardCard title="Contact us">
        <Typography>
          <b>User:</b> {user?.email} - {isAuthenticated ? "Authenticated" : "Not Authenticated"}
          <br />
          <b>Title:</b> Contact Us
          <br />
          <b>Description:</b> Provides contact information and a form for inquiries. Can also include a map for physical locations.
          <br />
          <b>Goal:</b> Encourage potential customers or partners to reach out for more information or support.
          <br />
          <b>Connections:</b> Linked from the footer of all pages and possibly the Home Page.
        </Typography>
      </DashboardCard>
    </PageContainer>
  )
}

export default ContactUsPage

