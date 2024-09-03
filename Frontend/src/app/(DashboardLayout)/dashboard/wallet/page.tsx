'use client';
import PageContainer from '@/app/(DashboardLayout)/components/container/PageContainer';
import DashboardCard from '@/app/(DashboardLayout)/components/shared/DashboardCard';
import { withAuthenticationRequired } from '@auth0/auth0-react';
import { Typography } from '@mui/material';

const WalletPage = () => {

  return (
    <PageContainer title="Organization" description="this is Organization page">
      <DashboardCard title="Organization">
        <Typography>

          <b>Description:</b> Organizations can put credits on their wallet to pay for the QR codes they create.
        </Typography>
      </DashboardCard>
    </PageContainer>
  );
};

export default withAuthenticationRequired(WalletPage);

