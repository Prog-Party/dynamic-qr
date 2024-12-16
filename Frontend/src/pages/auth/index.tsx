"use client"
import PageContainer from "@/components/container/PageContainer"
import { Box, Grid, Typography } from "@mui/material"
// components
import GenerateStaticQr from "@/components/generate/GenerateStaticQR"
import DashboardCard from "@/components/shared/DashboardCard"

const Dashboard = () => {

  return (
    <PageContainer title="Dashboard" description="this is Dashboard">
      <Box>
        <Grid container spacing={3}>
          <Grid item xs={12} lg={12}>
            <DashboardCard title="DASHBOARD!!! When logged in">
              <Typography>
                <b>Title:</b> Home
                <br />
                <b>Description:</b> The central hub for your website. Introduces the service with a compelling headline, a brief overview of dynamic QR codes, and a clear call-to-action (CTA) button that leads to the signup or demo page.
                <br />
                <b>Goal:</b> Provide an immediate understanding of what the service offers and direct users to take action (e.g., Sign Up, Learn More).
              </Typography>
            </DashboardCard>
          </Grid>
          <Grid item xs={12} lg={12}>
            <DashboardCard title="Generate static QR ">
              <>
                <Typography>
                  Generate your static QR code now. It can be anything, from text to an url.
                </Typography>
                <GenerateStaticQr />
              </>
            </DashboardCard>
          </Grid>
        </Grid>
      </Box>
    </PageContainer>
  )
}

export default Dashboard
