import type { AuthorDto } from "./blogTypes";

export type  UserGuestbookEntryResponse = {
    id: string;
    message: string;
    createdAt: Date;
    author: AuthorDto;
};

export type CreateGuestbookEntryRequest = {
    message: string;
};

export type UpdateGuestbookEntryRequest = {
    message: string;
};

export type GuestbookentryRequest = {
    id: string;
    message: string;
};
