import { required, maxLength, minLength } from '@vuelidate/validators'

export const AccountValidator =
{
	AccountName:        { required, maxLength: maxLength(50) },
	Email:              { required },
	PhoneNumber:        { required },
	StreetAddress:      { required, maxLength: maxLength(50) },
	City:               { required, maxLength: maxLength(50) },
	State:              { required, maxLength: maxLength(2) },
	PostalCode:         { required, maxLength: maxLength(10) },
}

export const ItemValidator =
{
	ItemId:             {  },
	ItemName:           { minLength: minLength(2), maxLength: maxLength(50) },
}

export const Team_MemberValidator =
{
	UserName:           { minLength: minLength(8), maxLength: maxLength(50) },
}
