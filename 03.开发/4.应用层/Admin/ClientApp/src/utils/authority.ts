import { reloadAuthorized } from './Authorized';
import { getCookie, setCookie, clearCookie } from './cookie';

// use localStorage to store the authority info, which might be sent from server in actual project.
export function getAuthority(str?: string): string | string[] {
  const authorityString =
    typeof str === 'undefined' && localStorage ? localStorage.getItem('antd-pro-authority') : str;
  // authorityString could be admin, "admin", ["admin"]
  let authority;
  try {
    if (authorityString) {
      authority = JSON.parse(authorityString);
    }
  } catch (e) {
    authority = authorityString;
  }
  if (typeof authority === 'string') {
    return [authority];
  }
  return authority;
}

export function setAuthority(authority: string | string[]): void {
  const proAuthority = typeof authority === 'string' ? [authority] : authority;
  localStorage.setItem('antd-pro-authority', JSON.stringify(proAuthority));
  // auto reload
  reloadAuthorized();
}

export function getToken(): string | undefined {
  const token: any = getCookie('token');
  return token;
}

export function setToken(token: string): void {
  if (typeof token === 'undefined' || token === '' || token === null) {
    clearToken();
  }
  setCookie('token', token);
}

export function clearToken(): void {
  setCookie('token', '');
}
