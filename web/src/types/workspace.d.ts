export interface IWorkspace {
    id: number
    name: string
    size: number
}

export interface IWorkspaceStructItem {
    type: 'file' | 'directory'
    name: string
    content: string
    items?: Array<IWorkspaceStructItem>
}