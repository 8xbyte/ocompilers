import { PayloadAction, createSlice, current } from "@reduxjs/toolkit"
import { createWorkspace, deleteWorkspace, getWorkspaces, renameWorkspace } from "../../api/workspace"
import { Status } from "../../types/status"
import { IWorkspace, IWorkspaceStructItem } from "../../types/workspace"

interface IInitialState {
    current: number | null
    get: {
        status: Status
        result: Array<IWorkspace>,
        error: string | null
    }
    getStruct: {
        status: Status
        result: Array<IWorkspaceStructItem>
        error: string | null
    }
    create: {
        status: Status
        result: IWorkspace | null,
        error: string | null
    }
    rename: {
        status: Status
        result: IWorkspace | null,
        error: string | null
    }
    delete: {
        status: Status
        result: IWorkspace | null,
        error: string | null
    }
}

const initialState: IInitialState = {
    current: null,
    get: {
        status: 'none',
        result: [],
        error: null
    },
    getStruct: {
        status: 'none',
        result: [],
        error: null
    },
    create: {
        status: 'none',
        result: null,
        error: null
    },
    rename: {
        status: 'none',
        result: null,
        error: null
    },
    delete: {
        status: 'none',
        result: null,
        error: null
    }
}

const workspaceSlice = createSlice({
    name: 'workspace',
    initialState,
    reducers: {
        workspaceSetCurrent(state, action: PayloadAction<number | null>) {
            state.current = action.payload
        }
    },
    extraReducers(builder) {
        builder.addCase(getWorkspaces.pending, (state, action) => {
            state.get.status = 'loading'
        }).addCase(getWorkspaces.fulfilled, (state, action) => {
            state.get.status = 'success'
            state.get.result = action.payload
        }).addCase(getWorkspaces.rejected, (state, action) => {
            state.get.status = 'failed'
            state.get.error = action.payload as string
        })

        builder.addCase(createWorkspace.pending, (state, action) => {
            state.create.status = 'loading'
        }).addCase(createWorkspace.fulfilled, (state, action) => {
            state.create.status = 'success'
            state.create.result = action.payload
            state.get.result.push(action.payload)
        }).addCase(createWorkspace.rejected, (state, action) => {
            state.create.status = 'failed'
            state.create.error = action.payload as string
        })

        builder.addCase(renameWorkspace.pending, (state, action) => {
            state.rename.status = 'loading'
        }).addCase(renameWorkspace.fulfilled, (state, action) => {
            state.rename.status = 'success'
            state.rename.result = action.payload
        }).addCase(renameWorkspace.rejected, (state, action) => {
            state.rename.status = 'failed'
            state.rename.error = action.payload as string
        })

        builder.addCase(deleteWorkspace.pending, (state, action) => {
            state.delete.status = 'loading'
        }).addCase(deleteWorkspace.fulfilled, (state, action) => {
            state.delete.status = 'success'
            state.delete.result = action.payload
            state.get.result = state.get.result.filter(item => item.id !== action.payload.id)
        }).addCase(deleteWorkspace.rejected, (state, action) => {
            state.delete.status = 'failed'
            state.delete.error = action.payload as string
        })
    },
})

export const { workspaceSetCurrent } = workspaceSlice.actions
export default workspaceSlice.reducer