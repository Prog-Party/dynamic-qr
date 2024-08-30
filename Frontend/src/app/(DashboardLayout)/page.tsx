'use client'
import PageContainer from '@/app/(DashboardLayout)/components/container/PageContainer';
import { Box, Grid, Typography } from '@mui/material';
// components
import Blog from '@/app/(DashboardLayout)/components/dashboard/Blog';
import MonthlyEarnings from '@/app/(DashboardLayout)/components/dashboard/MonthlyEarnings';
import ProductPerformance from '@/app/(DashboardLayout)/components/dashboard/ProductPerformance';
import RecentTransactions from '@/app/(DashboardLayout)/components/dashboard/RecentTransactions';
import SalesOverview from '@/app/(DashboardLayout)/components/dashboard/SalesOverview';
import YearlyBreakup from '@/app/(DashboardLayout)/components/dashboard/YearlyBreakup';
import DashboardCard from './components/shared/DashboardCard';
import Menuitems from './layout/sidebar/MenuItems';

const Dashboard = () => {


  return (
    <PageContainer title="Dashboard" description="this is Dashboard">
      <Box>
        <Grid container spacing={3}>
          <Grid item xs={12} lg={12}>
            <DashboardCard title="Goals">
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
            <DashboardCard title="Template Menu ">
              <ul>

                {Menuitems.map((item) => {

                  if (item.subheader)
                    return <></>

                  return <li key={item.id}>
                    <Box component={item.icon} sx={{ mr: 1 }} />
                    <a href={item.href}>{item.title}</a>
                  </li>
                })}
              </ul>
            </DashboardCard>
          </Grid>
          <Grid item xs={12} lg={8}>
            <SalesOverview />
          </Grid>
          <Grid item xs={12} lg={4}>
            <Grid container spacing={3}>
              <Grid item xs={12}>
                <YearlyBreakup />
              </Grid>
              <Grid item xs={12}>
                <MonthlyEarnings />
              </Grid>
            </Grid>
          </Grid>
          <Grid item xs={12} lg={4}>
            <RecentTransactions />
          </Grid>
          <Grid item xs={12} lg={8}>
            <ProductPerformance />
          </Grid>
          <Grid item xs={12}>
            <Blog />
          </Grid>
        </Grid>
      </Box>
    </PageContainer>
  )
}

export default Dashboard;
