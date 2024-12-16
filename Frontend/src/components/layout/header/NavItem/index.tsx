import React from "react"
// mui imports
import {
  List,
  ListItem,
  ListItemButton,
  ListItemText,
  styled,
  Typography,
  useTheme
} from "@mui/material"
import Link from "next/link"

type NavGroup = {
  [x: string]: any;
  id?: string;
  navlabel?: boolean;
  subheader?: string;
  title?: string;
  href?: any;
  onClick?: React.MouseEvent<HTMLButtonElement, MouseEvent>;
};

interface ItemType {
  item: NavGroup;
  onClick: (event: React.MouseEvent<HTMLElement>) => void;
  hideMenu?: any;
  level?: number | any;
  pathDirect: string;
}

const NavItem = ({ item, level, pathDirect, onClick }: ItemType) => {
  const Icon = item.icon
  const theme = useTheme()

  const ListItemStyled = styled(ListItem)(() => ({
    padding: 0,
    ".MuiButtonBase-root": {
      whiteSpace: "nowrap",
      marginBottom: "2px",
      padding: "8px 10px",
      borderRadius: "8px",
      backgroundColor: level > 1 ? "transparent !important" : "inherit",
      color: theme.palette.text.secondary,
      paddingLeft: "10px",
      "&:hover": {
        textDecoration: "underline",
        color: theme.palette.primary.main,
      },
      "&.Mui-selected": {
        textDecoration: "underline",
        color: theme.palette.primary.main,
        backgroundColor: "inherit",

        "&:hover": {
          backgroundColor: "inherit",
        },
      },
    },
  }))

  return (
    <List
      component="div"
      disablePadding
      key={item.id}
      sx={{ float: "left" }}>
      <ListItemStyled>
        <ListItemButton
          component={Link}
          href={item.href}
          disabled={item.disabled}
          selected={pathDirect === item.href}
          target={item.external ? "_blank" : ""}
          onClick={onClick}
        >
          <ListItemText>
            <Typography
              variant="body2"
              sx={{
                fontSize: "1.25rem",
                color: (theme) => theme.palette.primary.main
              }}>
              {item.title}
            </Typography>
          </ListItemText>
        </ListItemButton>
      </ListItemStyled>
    </List>
  )
}

export default NavItem
