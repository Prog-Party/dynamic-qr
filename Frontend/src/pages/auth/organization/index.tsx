"use client"
import { createCode, getCodes } from "@/api/qr-codes"
import PageContainer from "@/components/container/PageContainer"
import DashboardCard from "@/components/shared/DashboardCard"
import { useAuth0, withAuthenticationRequired } from "@auth0/auth0-react"
import { Button, Typography } from "@mui/material"
import Link from "next/link"
import { useEffect, useState } from "react"

const OrganizationPage = () => {

  const { user } = useAuth0()
  const [qrCodes, setQrCodes] = useState()

  const createQrCode = async () => {
    const result = await createCode(user!.organizationId, "temp")
  }

  useEffect(() => {
    const fetchData = async () => {
      const apiResult = await getCodes(user!.organizationId)
      setQrCodes(apiResult)
    }

    fetchData()
  }, [])

  return (
    <PageContainer title="Organization" description="this is Organization page">
      <DashboardCard title="Organization">
        <>
          <ul>
            <li>Organization ID: {user?.organizationId}</li>
            <li>Name: {user?.nickname}</li>
            <li>E-mail: {user?.email}</li>
          </ul>
          <Typography>
            <b>Description:</b> Information about this person&apos;s organization. Invite colleagues, maybe some rolebased stuff, etc.
          </Typography>
          <p>
            QR Codes: {qrCodes}
          </p>
        </>
      </DashboardCard>
      <DashboardCard title="Overview QR Codes">
        <Link href="/auth/qr-codes/c57fc106-b7f9-4f48-b70c-f0ea1fe39752">
          QR Code 1 (id c57fc106-b7f9-4f48-b70c-f0ea1fe39752)
        </Link>
      </DashboardCard>
      <DashboardCard title="Create QR Code">
        <Button variant="contained" color="primary" onClick={createQrCode}>
          Create QR Code
        </Button>
      </DashboardCard>
    </PageContainer>
  )
}

export default withAuthenticationRequired(OrganizationPage)

