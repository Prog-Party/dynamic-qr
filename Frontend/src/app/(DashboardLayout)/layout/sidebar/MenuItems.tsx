import {
  IconCopy,
  IconLayoutDashboard,
  IconMoodHappy,
  IconTypography
} from "@tabler/icons-react";

import { uniqueId } from "lodash";

const Menuitems = [
  {
    navlabel: true,
    subheader: "Organization stuff",
  },

  {
    id: uniqueId(),
    title: "Organization",
    icon: IconLayoutDashboard,
    href: "/dashboard/organization",
  },

  {
    id: uniqueId(),
    title: "Wallet",
    icon: IconLayoutDashboard,
    href: "/dashboard/wallet",
  },
  {
    navlabel: true,
    subheader: "Design",
  },
  {
    id: uniqueId(),
    title: "Typography",
    icon: IconTypography,
    href: "/design/typography",
  },
  {
    id: uniqueId(),
    title: "Shadow",
    icon: IconCopy,
    href: "/design/shadow",
  },
  {
    id: uniqueId(),
    title: "Icons",
    icon: IconMoodHappy,
    href: "/design/icons",
  }
];

export default Menuitems;
