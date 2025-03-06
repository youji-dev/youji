export default interface SearchTicketsRequest {
    filters?: Filters,
    orderByColumn: string,
    orderDesc: boolean,
    skip: number,
    take: number,
    useOr: boolean
}

interface Filters {
    [key: string]: string[]
}