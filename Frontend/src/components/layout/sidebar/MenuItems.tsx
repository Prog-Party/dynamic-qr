import {
  IconLayoutDashboard
} from "@tabler/icons-react"

import { uniqueId } from "lodash"

const Menuitems = [
  {
    id: uniqueId(),
    title: "Home",
    icon: IconLayoutDashboard,
    href: "/",
  },
  {
    navlabel: true,
    subheader: "Organization stuff",
  },
  {
    id: uniqueId(),
    title: "Organization",
    icon: IconLayoutDashboard,
    href: "/auth/organization",
  },
  {
    id: uniqueId(),
    title: "Wallet",
    icon: IconLayoutDashboard,
    href: "/auth/wallet",
  }
]

export default Menuitems
