export interface IWorkspaceStructGet {
    id: number
}

export interface IWorkspaceStructCreateFileRequest {
    workspaceId: number
    path: string
}

export interface IWorkspaceStructCreateFileResponse {
    path: string
}

export interface IWorkspaceStructReadFileRequest {
    workspaceId: number
    path: string
}

export interface IWorkspaceStructReadFileResponse {
    path: string
    content: string
}

export interface IWorkspaceStructWriteFileRequest {
    workspaceId: number
    path: string
    content: string
}

export interface IWorkspaceStructWriteFileResponse {
    path: string
}

export interface IWorkspaceStructRenameFileRequest {
    workspaceId: number
    oldPath: string
    newPath: string
}

export interface IWorkspaceStructRenameFileResponse {
    path: string
}

export interface IWorkspaceStructDeleteFileRequest {
    workspaceId: number
    path: string
}

export interface IWorkspaceStructDeleteFileResponse {
    path: string
}

export interface IWorkspaceStructCreateDirectoryRequest {
    workspaceId: number
    path: string
}

export interface IWorkspaceStructCreateDirectoryResponse {
    path: string
}

export interface IWorkspaceStructRenameDirectoryRequest {
    workspaceId: number
    oldPath: string
    newPath: string
}

export interface IWorkspaceStructRenameDirectoryResponse {
    path: string
}

export interface IWorkspaceStructDeleteDirectoryRequest {
    workspaceId: number
    path: string
}

export interface IWorkspaceStructDeleteDirectoryResponse {
    path: string
}