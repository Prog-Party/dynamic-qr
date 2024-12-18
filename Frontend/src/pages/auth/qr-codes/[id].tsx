"use client"
import { getCode } from "@/api/qr-code/qr-code"
import PageContainer from "@/components/container/PageContainer"
import DashboardCard from "@/components/shared/DashboardCard"
import { useAuth0, withAuthenticationRequired } from "@auth0/auth0-react"
import { useRouter } from "next/router"
import { useEffect, useState } from "react"

const OrganizationPage = () => {

  const router = useRouter()
  const { id } = router.query

  const { user } = useAuth0()
  const organizationId = user?.organizationId ?? -1
  const [qrCode, setQrCode] = useState()

  const createQrCode = async () => {
    const result = await getCode(user!.organizationId, "temp")
  }

  useEffect(() => {
    const fetchData = async () => {
      const apiResult = await getCode(organizationId, id as string)
      setQrCode(apiResult)
    }

    fetchData()
  }, [])

  return (
    <PageContainer title="QR code" description="welcome to a qr code page">
      <DashboardCard title="QR code information">
        <>
            Info over qr code
        </>
      </DashboardCard>
    </PageContainer>
  )
}

export default withAuthenticationRequired(OrganizationPage)

