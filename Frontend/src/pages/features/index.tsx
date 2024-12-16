"use client"
import PageContainer from "@/components/container/PageContainer"
import DashboardCard from "@/components/shared/DashboardCard"
import { Typography } from "@mui/material"

const FeaturesPage = () => {
  return (
    <PageContainer title="Features" description="this is Features page">
      <DashboardCard title="Features">
        <Typography>
          <b>Title:</b> Features
          <br />
          <b>Description:</b> Details the core features of your dynamic QR code service, such as editable content, analytics, tracking, and custom branding.
          <br />
          <b>Goal:</b> Showcase the unique selling points of your service to convince visitors of its value.
          <br />
          <b>Connections:</b> Links from the Home Page, leads to the &quot;Pricing&quot; and &quot;How It Works&quot; pages.
        </Typography>
      </DashboardCard>
    </PageContainer>
  )
}

export default FeaturesPage

