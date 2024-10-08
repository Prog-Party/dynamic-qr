'use client';
import PageContainer from '@/app/(DashboardLayout)/components/container/PageContainer';
import DashboardCard from '@/app/(DashboardLayout)/components/shared/DashboardCard';
import { useAuth0, withAuthenticationRequired } from '@auth0/auth0-react';
import { Typography } from '@mui/material';

const OrganizationPage = () => {

  const { user } = useAuth0();

  return (
    <PageContainer title="Organization" description="this is Organization page">
      <DashboardCard title="Organization">
        <>
          <ul>
            <li>Name: {user?.nickname}</li>
            <li>E-mail: {user?.email}</li>
          </ul>
          <Typography>
            <b>Description:</b> Information about this person&apos;s organization. Invite colleagues, maybe some rolebased stuff, etc.
          </Typography>
        </>
      </DashboardCard>
    </PageContainer>
  );
};

export default withAuthenticationRequired(OrganizationPage);

