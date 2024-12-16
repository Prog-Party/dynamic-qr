"use client"
import PageContainer from "@/components/container/PageContainer"
import DashboardCard from "@/components/shared/DashboardCard"
import { withAuthenticationRequired } from "@auth0/auth0-react"
import { Typography } from "@mui/material"

const WalletPage = () => {

  return (
    <PageContainer title="Wallet" description="this is the Wallet page">
      <DashboardCard title="Wallet">
        <Typography>
          <b>Description:</b> Organizations can put credits on their wallet to pay for the QR codes they create.
        </Typography>
      </DashboardCard>
    </PageContainer>
  )
}

export default withAuthenticationRequired(WalletPage)

