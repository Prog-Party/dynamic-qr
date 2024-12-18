/** @type {import('next').NextConfig} */
const nextConfig = {
  reactStrictMode: true,
  env: {
    BACKEND_KEY_QUERY: process.env.BACKEND_KEY_QUERY
  },
}

module.exports = nextConfig
