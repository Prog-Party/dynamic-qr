export type GenerateStaticQRContentProps = {
    value: string,
    setValue: (value: string) => void
};

//ADR;TYPE=home:;;123 Main St.;Springfield;IL;12345;USA

export type Address = {
    /// The type of address. For example, "home", "work", etc.
    type: string,
    street?: string,
    city?: string,
    state?: string,
    zip?: string,
    country?: string
};