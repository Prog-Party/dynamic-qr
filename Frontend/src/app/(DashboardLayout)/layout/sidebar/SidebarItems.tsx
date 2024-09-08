import { Box, List, useMediaQuery } from "@mui/material"
import { usePathname } from "next/navigation"
import { default as HeaderMenuItems } from "../header/MenuItems"
import Menuitems from "./MenuItems"
import NavGroup from "./NavGroup/NavGroup"
import NavItem from "./NavItem"

const SidebarItems = ({ toggleMobileSidebar }: any) => {
  const pathname = usePathname()
  const pathDirect = pathname

  const lgUp = useMediaQuery((theme: any) => theme.breakpoints.up("lg"))

  return (
    <Box sx={{ px: 3 }}>
      <List sx={{ pt: 0 }} className="sidebarNav" component="div">

        {/* Only show the headermenu on mobile */}
        {!lgUp && HeaderMenuItems.map((item) => {
          return (
            <NavItem
              item={item}
              key={item.id}
              pathDirect={pathDirect}
              onClick={toggleMobileSidebar}
            />
          )
        })}

        {Menuitems.map((item) => {
          // {/********SubHeader**********/}
          if (item.subheader) {
            return <NavGroup item={item} key={item.subheader} />

            // {/********If Sub Menu**********/}
            /* eslint no-else-return: "off" */
          } else {
            return (
              <NavItem
                item={item}
                key={item.id}
                pathDirect={pathDirect}
                onClick={toggleMobileSidebar}
              />
            )
          }
        })}
      </List>
    </Box>
  )
}
export default SidebarItems
