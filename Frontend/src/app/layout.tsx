"use client";
import { baselightTheme } from "@/utils/theme/DefaultColors";
import { Auth0Provider } from '@auth0/auth0-react';
import CssBaseline from "@mui/material/CssBaseline";
import { ThemeProvider } from "@mui/material/styles";

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <html lang="en">
      <head>
        <link rel="icon" href="/images/logos/Logo-16x16.ico" />
      </head>
      <body>
        <Auth0Provider
          domain="https://dev-bha1i8wi67urwpal.eu.auth0.com"
          clientId="HjBsSLqm8VnqjEmZbmju2eWMHE58JNlQ"
          authorizationParams={{
            redirect_uri:
              typeof window !== 'undefined' ? window.location.origin : undefined,
          }}
        >
          <ThemeProvider theme={baselightTheme}>
            {/* CssBaseline kickstart an elegant, consistent, and simple baseline to build upon. */}
            <CssBaseline />
            {children}
          </ThemeProvider>
        </Auth0Provider>
      </body>
    </html>
  );
}
