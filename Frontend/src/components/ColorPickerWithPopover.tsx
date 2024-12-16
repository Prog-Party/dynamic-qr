import { Popover } from "@mui/material"
import React, { useState } from "react"
import { ColorResult, SketchPicker } from "react-color"

type ColorPickerWithPopoverProps = {
    color: string;
    size?: string;
    onChange: (color: string) => void;
};

const ColorPickerWithPopover = (props: ColorPickerWithPopoverProps) => {
  const [color, setColor] = useState<string>(props.color)
  const [anchorEl, setAnchorEl] = useState<HTMLElement | null>(null)

  const width = props.size ?? "36px"
  const height = props.size ?? "36px"

  const handleClick = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget)
  }

  const handleClose = () => {
    setAnchorEl(null)
  }

  const handleChange = (color: ColorResult) => {
    setColor(color.hex)
    props.onChange(color.hex)
  }

  const open = Boolean(anchorEl)
  const id = open ? "color-picker-popover" : undefined

  return (
    <div>
      <div
        style={{
          backgroundColor: color,
          width: width,
          height: height,
          cursor: "pointer",
          border: "2px solid #000000",
        }}
        onClick={handleClick}
      />
      <Popover
        id={id}
        open={open}
        anchorEl={anchorEl}
        onClose={handleClose}
        anchorOrigin={{
          vertical: "bottom",
          horizontal: "left",
        }}
        transformOrigin={{
          vertical: "top",
          horizontal: "left",
        }}
      >
        <SketchPicker color={color} onChange={handleChange} />
      </Popover>
    </div>
  )
}

export default ColorPickerWithPopover