import { createAsyncThunk } from "@reduxjs/toolkit"
import axios, { AxiosError } from "axios"
import { setIsAuth } from "../store/slices/authSlice"
import { IError } from "../types/error"
import { IWorkspaceStructItem } from "../types/workspace"
import { IWorkspaceStructCreateDirectoryRequest, IWorkspaceStructCreateDirectoryResponse, IWorkspaceStructCreateFileRequest, IWorkspaceStructCreateFileResponse, IWorkspaceStructDeleteDirectoryRequest, IWorkspaceStructDeleteDirectoryResponse, IWorkspaceStructDeleteFileRequest, IWorkspaceStructDeleteFileResponse, IWorkspaceStructGet, IWorkspaceStructReadFileRequest, IWorkspaceStructReadFileResponse, IWorkspaceStructRenameDirectoryRequest, IWorkspaceStructRenameDirectoryResponse, IWorkspaceStructRenameFileRequest, IWorkspaceStructRenameFileResponse, IWorkspaceStructWriteFileRequest, IWorkspaceStructWriteFileResponse } from "../types/workspaceStruct"

export const getWorkspaceStruct = createAsyncThunk('workspace/struct/get', async (data: IWorkspaceStructGet, thunkApi) => {
    try {
        const response = await axios.get<Array<IWorkspaceStructItem>>('https://astra.oregona.ru/api/workspace/struct/get', {
            params: data,
            withCredentials: true
        })
        return thunkApi.fulfillWithValue(response.data)
    } catch (error) {
        if (axios.isAxiosError(error)) {
            const axiosError = error as AxiosError<IError>
            if (axiosError.response?.status === 403) thunkApi.dispatch(setIsAuth(false))
            return thunkApi.rejectWithValue(axiosError.response?.data.message)
        } else {
            return thunkApi.rejectWithValue(error)
        }
    }
})

export const createFileWorkspaceStruct = createAsyncThunk('workspace/struct/file/create', async (data: IWorkspaceStructCreateFileRequest, thunkApi) => {
    try {
        const response = await axios.post<IWorkspaceStructCreateFileResponse>('https://astra.oregona.ru/api/workspace/struct/file/create', data, {
            withCredentials: true
        })
        return thunkApi.fulfillWithValue(response.data)
    } catch (error) {
        if (axios.isAxiosError(error)) {
            const axiosError = error as AxiosError<IError>
            if (axiosError.response?.status === 403) thunkApi.dispatch(setIsAuth(false))
            return thunkApi.rejectWithValue(axiosError.response?.data.message)
        } else {
            return thunkApi.rejectWithValue(error)
        }
    }
})

export const readFileWorkspaceStruct = createAsyncThunk('workspace/struct/file/read', async (data: IWorkspaceStructReadFileRequest, thunkApi) => {
    try {
        const response = await axios.post<IWorkspaceStructReadFileResponse>('https://astra.oregona.ru/api/workspace/struct/file/read', data, {
            withCredentials: true
        })
        return thunkApi.fulfillWithValue(response.data)
    } catch (error) {
        if (axios.isAxiosError(error)) {
            const axiosError = error as AxiosError<IError>
            if (axiosError.response?.status === 403) thunkApi.dispatch(setIsAuth(false))
            return thunkApi.rejectWithValue(axiosError.response?.data.message)
        } else {
            return thunkApi.rejectWithValue(error)
        }
    }
})

export const writeFileWorkspaceStruct = createAsyncThunk('workspace/struct/file/write', async (data: IWorkspaceStructWriteFileRequest, thunkApi) => {
    try {
        const response = await axios.post<IWorkspaceStructWriteFileResponse>('https://astra.oregona.ru/api/workspace/struct/file/write', data, {
            withCredentials: true
        })
        return thunkApi.fulfillWithValue(response.data)
    } catch (error) {
        if (axios.isAxiosError(error)) {
            const axiosError = error as AxiosError<IError>
            if (axiosError.response?.status === 403) thunkApi.dispatch(setIsAuth(false))
            return thunkApi.rejectWithValue(axiosError.response?.data.message)
        } else {
            return thunkApi.rejectWithValue(error)
        }
    }
})

export const renameFileWorkspaceStruct = createAsyncThunk('workspace/struct/file/rename', async (data: IWorkspaceStructRenameFileRequest, thunkApi) => {
    try {
        const response = await axios.post<IWorkspaceStructRenameFileResponse>('https://astra.oregona.ru/api/workspace/struct/file/rename', data, {
            withCredentials: true
        })
        return thunkApi.fulfillWithValue(response.data)
    } catch (error) {
        if (axios.isAxiosError(error)) {
            const axiosError = error as AxiosError<IError>
            if (axiosError.response?.status === 403) thunkApi.dispatch(setIsAuth(false))
            return thunkApi.rejectWithValue(axiosError.response?.data.message)
        } else {
            return thunkApi.rejectWithValue(error)
        }
    }
})

export const deleteFileWorkspaceStruct = createAsyncThunk('workspace/struct/file/delete', async (data: IWorkspaceStructDeleteFileRequest, thunkApi) => {
    try {
        const response = await axios.post<IWorkspaceStructDeleteFileResponse>('https://astra.oregona.ru/api/workspace/struct/file/delete', data, {
            withCredentials: true
        })
        return thunkApi.fulfillWithValue(response.data)
    } catch (error) {
        if (axios.isAxiosError(error)) {
            const axiosError = error as AxiosError<IError>
            if (axiosError.response?.status === 403) thunkApi.dispatch(setIsAuth(false))
            return thunkApi.rejectWithValue(axiosError.response?.data.message)
        } else {
            return thunkApi.rejectWithValue(error)
        }
    }
})

export const createDirectoryWorkspaceStruct = createAsyncThunk('workspace/struct/directory/create', async (data: IWorkspaceStructCreateDirectoryRequest, thunkApi) => {
    try {
        const response = await axios.post<IWorkspaceStructCreateDirectoryResponse>('https://astra.oregona.ru/api/workspace/struct/directory/create', data, {
            withCredentials: true
        })
        return thunkApi.fulfillWithValue(response.data)
    } catch (error) {
        if (axios.isAxiosError(error)) {
            const axiosError = error as AxiosError<IError>
            if (axiosError.response?.status === 403) thunkApi.dispatch(setIsAuth(false))
            return thunkApi.rejectWithValue(axiosError.response?.data.message)
        } else {
            return thunkApi.rejectWithValue(error)
        }
    }
})

export const renameDirectoryWorkspaceStruct = createAsyncThunk('workspace/struct/directory/rename', async (data: IWorkspaceStructRenameDirectoryRequest, thunkApi) => {
    try {
        const response = await axios.post<IWorkspaceStructRenameDirectoryResponse>('https://astra.oregona.ru/api/workspace/struct/directory/create', data, {
            withCredentials: true
        })
        return thunkApi.fulfillWithValue(response.data)
    } catch (error) {
        if (axios.isAxiosError(error)) {
            const axiosError = error as AxiosError<IError>
            if (axiosError.response?.status === 403) thunkApi.dispatch(setIsAuth(false))
            return thunkApi.rejectWithValue(axiosError.response?.data.message)
        } else {
            return thunkApi.rejectWithValue(error)
        }
    }
})

export const deleteDirectoryWorkspaceStruct = createAsyncThunk('workspace/struct/directory/delete', async (data: IWorkspaceStructDeleteDirectoryRequest, thunkApi) => {
    try {
        const response = await axios.post<IWorkspaceStructDeleteDirectoryResponse>('https://astra.oregona.ru/api/workspace/struct/directory/delete', data, {
            withCredentials: true
        })
        return thunkApi.fulfillWithValue(response.data)
    } catch (error) {
        if (axios.isAxiosError(error)) {
            const axiosError = error as AxiosError<IError>
            if (axiosError.response?.status === 403) thunkApi.dispatch(setIsAuth(false))
            return thunkApi.rejectWithValue(axiosError.response?.data.message)
        } else {
            return thunkApi.rejectWithValue(error)
        }
    }
})