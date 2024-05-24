import { createAsyncThunk } from "@reduxjs/toolkit";
import axios, { AxiosError } from "axios";
import { setIsAuth } from "../store/slices/authSlice";
import { IError } from "../types/error";
import { IWorkspace } from "../types/workspace";

export const getWorkspaces = createAsyncThunk('workspace/get', async (data, thunkApi) => {
    try {
        const response = await axios.get<Array<IWorkspace>>('https://astra.oregona.ru/api/workspace/get', {
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

interface ICreateWorkspaceData {
    name: string
}

export const createWorkspace = createAsyncThunk('workspace/create', async (data: ICreateWorkspaceData, thunkApi) => {
    try {
        const response = await axios.post<IWorkspace>('https://astra.oregona.ru/api/workspace/create', data, {
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

interface IRenameWorkspaceData {
    id: number
    newName: string
}

export const renameWorkspace = createAsyncThunk('workspace/rename', async (data: IRenameWorkspaceData, thunkApi) => {
    try {
        const response = await axios.post<IWorkspace>('https://astra.oregona.ru/api/workspace/rename', data, {
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

interface IDeleteWorkspaceData {
    id: number
}

export const deleteWorkspace = createAsyncThunk('workspace/delete', async (data: IDeleteWorkspaceData, thunkApi) => {
    try {
        const response = await axios.post<IWorkspace>('https://astra.oregona.ru/api/workspace/delete', data, {
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