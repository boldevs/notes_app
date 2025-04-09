import API from './api';
import type { LoginPayload } from '../types/auth';

export const login = async (payload: LoginPayload) => {
  const response = await API.post('/auth/login', payload);
  return response.data;
};

export const register = async (payload: LoginPayload) => {
  const response = await API.post('/auth/register', payload)
  return response.data
}