import { PayloadAction, createSlice } from "@reduxjs/toolkit"
import { createDirectoryWorkspaceStruct, createFileWorkspaceStruct, deleteDirectoryWorkspaceStruct, deleteFileWorkspaceStruct, getWorkspaceStruct, readFileWorkspaceStruct, renameDirectoryWorkspaceStruct, renameFileWorkspaceStruct, writeFileWorkspaceStruct } from "../../api/workspaceStruct"
import { Status } from "../../types/status"
import { IWorkspaceStructItem } from "../../types/workspace"
import { IWorkspaceStructCreateDirectoryResponse, IWorkspaceStructCreateFileResponse, IWorkspaceStructDeleteDirectoryResponse, IWorkspaceStructDeleteFileResponse, IWorkspaceStructReadFileResponse, IWorkspaceStructRenameDirectoryResponse, IWorkspaceStructRenameFileResponse, IWorkspaceStructWriteFileResponse } from "../../types/workspaceStruct"

interface IInitialState {
    files: Array<IWorkspaceStructItem>
    openned: string | null
    getStruct: {
        status: Status
        result: Array<IWorkspaceStructItem>
        error: string | null
    }
    createFile: {
        status: Status
        result: IWorkspaceStructCreateFileResponse | null
        error: string | null
    }
    readFile: {
        status: Status
        result: IWorkspaceStructReadFileResponse | null
        error: string | null
    }
    writeFile: {
        status: Status
        result: IWorkspaceStructWriteFileResponse | null
        error: string | null
    }
    renameFile: {
        status: Status
        result: IWorkspaceStructRenameFileResponse | null
        error: string | null
    }
    deleteFile: {
        status: Status
        result: IWorkspaceStructDeleteFileResponse | null
        error: string | null
    }
    createDirectory: {
        status: Status
        result: IWorkspaceStructCreateDirectoryResponse | null
        error: string | null
    }
    renameDirectory: {
        status: Status
        result: IWorkspaceStructRenameDirectoryResponse | null
        error: string | null
    }
    deleteDirectory: {
        status: Status
        result: IWorkspaceStructDeleteDirectoryResponse | null
        error: string | null
    }
}

const initialState: IInitialState = {
    files: [],
    openned: null,
    getStruct: {
        status: 'none',
        result: [],
        error: null
    },
    createFile: {
        status: 'none',
        result: null,
        error: null
    },
    readFile: {
        status: 'none',
        result: null,
        error: null
    },
    writeFile: {
        status: 'none',
        result: null,
        error: null
    },
    renameFile: {
        status: 'none',
        result: null,
        error: null
    },
    deleteFile: {
        status: 'none',
        result: null,
        error: null
    },
    createDirectory: {
        status: 'none',
        result: null,
        error: null
    },
    renameDirectory: {
        status: 'none',
        result: null,
        error: null
    },
    deleteDirectory: {
        status: 'none',
        result: null,
        error: null
    }
}

const workspaceStructSlice = createSlice({
    name: 'workspace/struct',
    initialState,
    reducers: {
        workspaceClearFiles(state) {
            state.files = []
        },
        workspaceStructAddFile(state, action: PayloadAction<IWorkspaceStructItem>) {
            state.files.push({
                type: 'file',
                name: action.payload.name,
                content: action.payload.content
            })
        },
        workspaceStructDeleteFile(state, action: PayloadAction<string>) {
            state.files = state.files.filter((item) => item.name !== action.payload)
        },
        workspaceStructWriteFile(state, action: PayloadAction<string>) {
            state.files.forEach((item) => {
                if (item.name === state.openned) {
                    item.content = action.payload
                }
            })
        },
        workspaceStructSetOpennedFile(state, action: PayloadAction<string>) {
            state.openned = action.payload
        }
    },
    extraReducers(builder) {
        builder.addCase(getWorkspaceStruct.pending, (state, action) => {
            state.getStruct.status = 'loading'
        }).addCase(getWorkspaceStruct.fulfilled, (state, action) => {
            state.getStruct.status = 'success'
            state.getStruct.result = action.payload
        }).addCase(getWorkspaceStruct.rejected, (state, action) => {
            state.getStruct.status = 'failed'
            state.getStruct.error = action.payload as string
        })

        builder.addCase(createFileWorkspaceStruct.pending, (state, action) => {
            state.createFile.status = 'loading'
        }).addCase(createFileWorkspaceStruct.fulfilled, (state, action) => {
            state.createFile.status = 'success'
            state.createFile.result = action.payload
        }).addCase(createFileWorkspaceStruct.rejected, (state, action) => {
            state.createFile.status = 'failed'
            state.createFile.error = action.payload as string
        })

        builder.addCase(readFileWorkspaceStruct.pending, (state, action) => {
            state.readFile.status = 'loading'
        }).addCase(readFileWorkspaceStruct.fulfilled, (state, action) => {
            state.readFile.status = 'success'
            state.readFile.result = action.payload
        }).addCase(readFileWorkspaceStruct.rejected, (state, action) => {
            state.readFile.status = 'failed'
            state.readFile.error = action.payload as string
        })

        builder.addCase(writeFileWorkspaceStruct.pending, (state, action) => {
            state.writeFile.status = 'loading'
        }).addCase(writeFileWorkspaceStruct.fulfilled, (state, action) => {
            state.writeFile.status = 'success'
            state.writeFile.result = action.payload
        }).addCase(writeFileWorkspaceStruct.rejected, (state, action) => {
            state.writeFile.status = 'failed'
            state.writeFile.error = action.payload as string
        })

        builder.addCase(renameFileWorkspaceStruct.pending, (state, action) => {
            state.renameFile.status = 'loading'
        }).addCase(renameFileWorkspaceStruct.fulfilled, (state, action) => {
            state.renameFile.status = 'success'
            state.renameFile.result = action.payload
        }).addCase(renameFileWorkspaceStruct.rejected, (state, action) => {
            state.renameFile.status = 'failed'
            state.renameFile.error = action.payload as string
        })

        builder.addCase(deleteFileWorkspaceStruct.pending, (state, action) => {
            state.deleteFile.status = 'loading'
        }).addCase(deleteFileWorkspaceStruct.fulfilled, (state, action) => {
            state.deleteFile.status = 'success'
            state.deleteFile.result = action.payload
        }).addCase(deleteFileWorkspaceStruct.rejected, (state, action) => {
            state.deleteFile.status = 'failed'
            state.deleteFile.error = action.payload as string
        })

        builder.addCase(createDirectoryWorkspaceStruct.pending, (state, action) => {
            state.createDirectory.status = 'loading'
        }).addCase(createDirectoryWorkspaceStruct.fulfilled, (state, action) => {
            state.createDirectory.status = 'success'
            state.createDirectory.result = action.payload
        }).addCase(createDirectoryWorkspaceStruct.rejected, (state, action) => {
            state.createDirectory.status = 'failed'
            state.createDirectory.error = action.payload as string
        })

        builder.addCase(renameDirectoryWorkspaceStruct.pending, (state, action) => {
            state.renameDirectory.status = 'loading'
        }).addCase(renameDirectoryWorkspaceStruct.fulfilled, (state, action) => {
            state.renameDirectory.status = 'success'
            state.renameDirectory.result = action.payload
        }).addCase(renameDirectoryWorkspaceStruct.rejected, (state, action) => {
            state.renameDirectory.status = 'failed'
            state.renameDirectory.error = action.payload as string
        })

        builder.addCase(deleteDirectoryWorkspaceStruct.pending, (state, action) => {
            state.deleteDirectory.status = 'loading'
        }).addCase(deleteDirectoryWorkspaceStruct.fulfilled, (state, action) => {
            state.deleteDirectory.status = 'success'
            state.deleteDirectory.result = action.payload
        }).addCase(deleteDirectoryWorkspaceStruct.rejected, (state, action) => {
            state.deleteDirectory.status = 'failed'
            state.deleteDirectory.error = action.payload as string
        })
    },
})

export const { workspaceClearFiles, workspaceStructDeleteFile, workspaceStructAddFile, workspaceStructWriteFile, workspaceStructSetOpennedFile } = workspaceStructSlice.actions
export default workspaceStructSlice.reducer