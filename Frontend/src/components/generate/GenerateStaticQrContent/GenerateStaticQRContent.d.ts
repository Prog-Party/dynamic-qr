export type GenerateStaticQRContentProps = {
    value: string,
    setValue: (value: string) => void
};

export type Address = {
    // The type of address. For example, "home", "work", etc.
    type: string,
    street?: string,
    city?: string,
    state?: string,
    zip?: string,
    country?: string
};