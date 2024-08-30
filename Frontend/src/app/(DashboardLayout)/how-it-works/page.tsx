'use client';
import PageContainer from '@/app/(DashboardLayout)/components/container/PageContainer';
import DashboardCard from '@/app/(DashboardLayout)/components/shared/DashboardCard';
import { Typography } from '@mui/material';


const HowItWorksPage = () => {
  return (
    <PageContainer title="How It Works Page" description="this is How It Works page">
      <DashboardCard title="How It Works Page">
        <Typography>
          <b>Title:</b> How It Works
          <br />
          <b>Description:</b> A step-by-step guide explaining how users can create, edit, and manage their dynamic QR codes. May include diagrams, screenshots, or videos.
          <br />
          <b>Goal:</b> Educate visitors on the process of using the service, reducing hesitation or confusion.
          <br />
          <b>Connections:</b> Linked from the Home Page and Features Page, leads to the "Sign Up" and "Demo" pages.
        </Typography>
      </DashboardCard>
    </PageContainer>
  );
};

export default HowItWorksPage;

