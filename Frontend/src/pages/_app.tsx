import { baselightTheme } from "@/utils/theme/DefaultColors"
import { Auth0Provider } from "@auth0/auth0-react"
import CssBaseline from "@mui/material/CssBaseline"
import { ThemeProvider } from "@mui/material/styles"
import type { NextPage } from "next"
import type { AppProps } from "next/app"
import type { ReactElement, ReactNode } from "react"
import Layout from "./layout"
// import '../styles/globals.css'

export type NextPageWithLayout<P = {}, IP = P> = NextPage<P, IP> & {
  getLayout?: (page: ReactElement) => ReactNode
}

type AppPropsWithLayout = AppProps & {
  Component: NextPageWithLayout
}

// This default export is required in a new `pages/_app.js` file.
export default function MyApp({ Component, pageProps }: AppPropsWithLayout) {
  return <>
    <Auth0Provider
      domain="https://dev-bha1i8wi67urwpal.eu.auth0.com"
      clientId="HjBsSLqm8VnqjEmZbmju2eWMHE58JNlQ"
      authorizationParams={{
        redirect_uri:
            typeof window !== "undefined" ? window.location.origin : undefined,
      }}
    >
      <ThemeProvider theme={baselightTheme}>
        {/* CssBaseline kickstart an elegant, consistent, and simple baseline to build upon. */}
        <CssBaseline />
        <Layout>
          <Component {...pageProps} />
        </Layout>

      </ThemeProvider>
    </Auth0Provider>
  </>
}
