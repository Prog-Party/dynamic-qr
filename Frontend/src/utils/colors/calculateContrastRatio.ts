/**
 * Calculates the contrast ratio between two hex colors.
 *
 * @param hexColor1 - The first hex color (#RRGGBB).
 * @param hexColor2 - The second hex color (#RRGGBB).
 * @returns The contrast ratio between the two colors.
 */
export default function calculateContrastRatio(hexColor1: string, hexColor2: string): number {
  const lum1 = hexToLuminance(hexColor1)
  const lum2 = hexToLuminance(hexColor2)

  // Contrast ratio formula.
  return (Math.max(lum1, lum2) + 0.05) / (Math.min(lum1, lum2) + 0.05)
}

/**
* Convert a hex color (#RRGGBB) to its relative luminance value.
*/
function hexToLuminance(hex: string): number {
  // Convert hex to integer.
  const rgb = parseInt(hex.slice(1), 16)
  const r = (rgb >> 16) & 0xff
  const g = (rgb >> 8) & 0xff
  const b = rgb & 0xff

  /**
   * Convert RGB to sRGB and then to luminance.
   *
   * @param channel
   * @returns
   */
  const toLuminance = (channel: number) => {
    const srgb = channel / 255
    return srgb <= 0.03928 ? srgb / 12.92 : Math.pow((srgb + 0.055) / 1.055, 2.4)
  }

  // Calculate the luminance using the sRGB luminance formula.
  return 0.2126 * toLuminance(r) + 0.7152 * toLuminance(g) + 0.0722 * toLuminance(b)
}