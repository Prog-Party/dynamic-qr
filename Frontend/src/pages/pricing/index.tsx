"use client"
import PageContainer from "@/components/container/PageContainer"
import DashboardCard from "@/components/shared/DashboardCard"
import { Typography } from "@mui/material"

const PricingPage = () => {
  return (
    <PageContainer title="Pricing" description="this is Pricing page">
      <DashboardCard title="Pricing">
        <Typography>
          <b>Title:</b> Pricing
          <br />
          <b>Description:</b> Provides details on the pricing structure, including different plans, features per plan, and any free trials or discounts available.
          <br />
          <b>Goal:</b> Help users choose the right plan, pushing them towards conversion.
          <br />
          <b>Connections:</b> Linked from the Home Page, Features Page, and calls-to-action across the site. Leads to &quot;Sign Up.&quot;
        </Typography>
      </DashboardCard>
    </PageContainer>
  )
}

export default PricingPage

