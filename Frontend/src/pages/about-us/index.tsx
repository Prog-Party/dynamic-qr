"use client"
import PageContainer from "@/components/container/PageContainer"
import DashboardCard from "@/components/shared/DashboardCard"
import { Typography } from "@mui/material"

const AboutUsPage = () => {
  return (
    <PageContainer title="About us" description="this is About us page">
      <DashboardCard title="About us">
        <Typography>
          <b>Title:</b> About Us
          <br />
          <b>Description:</b> Provides information about the company, its mission, team, and history. Humanizes the brand and establishes credibility.
          <br />
          <b>Goal:</b> Build trust and convey the company’s values and vision.
          <br />
          <b>Connections:</b> Linked from the footer of all pages and possibly the Home Page.
        </Typography>
      </DashboardCard>
    </PageContainer>
  )
}

export default AboutUsPage

